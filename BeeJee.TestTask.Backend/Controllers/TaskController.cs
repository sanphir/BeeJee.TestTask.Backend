using BeeJee.TestTask.Backend.Config;
using BeeJee.TestTask.Backend.Dto;
using BeeJee.TestTask.Backend.Extensions;
using BeeJee.TestTask.DAL;
using BeeJee.TestTask.DAL.Models;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel;

namespace BeeJee.TestTask.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TestTaskDbContext _context;
        private readonly PageOptions _pageOptions;
        private readonly IValidator<NewTaskDto> _validator;
        private readonly ILogger<TaskController> _logger;
        public TaskController(TestTaskDbContext context, IOptions<PageOptions> pageOptions, IValidator<NewTaskDto> validator, ILogger<TaskController> logger)
        {
            _context = context;
            _pageOptions = pageOptions.Value;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseMessageDto<object>>> Get(int id, CancellationToken cancellationToken = default)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (task == null)
            {
                _logger.LogWarning("Задача c id=\"{id}\" не найдена.", id);

                return NotFound((new ResponseMessageDto<object>(ResponseStatus.Error)
                {
                    Message = $"Задача c id=\"{id}\" не найдена."
                }));
            }

            return Ok(new ResponseMessageDto<TaskDto>(ResponseStatus.Ok)
            {
                Message = task.Adapt<TaskModel, TaskDto>()
            });
        }

        [HttpGet]
        public async Task<ActionResult<ResponseMessageDto<object>>> Get(string sort_field, string sort_direction, int page, CancellationToken cancellationToken = default)
        {
            var totalCount = await _context.Tasks.CountAsync(cancellationToken);
            var skip = _pageOptions.PageSize * (page - 1);

            if ((page <= 0) || (skip > totalCount))
            {
                return Ok(new ResponseMessageDto<TasksResponseDto>(ResponseStatus.Ok)
                {
                    Message = new TasksResponseDto
                    {
                        Tasks = Array.Empty<TaskDto>(),
                        TotalTaskCount = totalCount,
                    }
                });
            }

            var prop = TypeDescriptor.GetProperties(typeof(TaskModel)).Find(sort_field ?? nameof(TaskModel.Id), ignoreCase: true)
                ?? throw new ArgumentException($"Ошибка сортировки: не найдено поле \"{sort_field}\"", nameof(sort_field));

            var ordered = (sort_direction?.ToLower() ?? "") switch
            {
                "desc" => _context.Tasks.OrderByDescending(prop.Name),
                "asc" => _context.Tasks.OrderBy(prop.Name),
                _ => throw new ArgumentException($"Неверное значение \"{sort_direction}\" направления сортировки", nameof(sort_direction))
            };

            var result = await ordered.AsNoTracking()
                .Skip(skip)
                .Take(_pageOptions.PageSize)
                .ToListAsync(cancellationToken);

            var mapedResult = result.Select(e => e.Adapt<TaskModel, TaskDto>());

            return Ok(new ResponseMessageDto<TasksResponseDto>(ResponseStatus.Ok)
            {
                Message = new TasksResponseDto
                {
                    Tasks = mapedResult,
                    TotalTaskCount = totalCount,
                }
            });
        }

        [HttpPost("create")]
        public async Task<ActionResult<ResponseMessageDto<object>>> Create(NewTaskDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseMessageDto<object>(ResponseStatus.Error)
                {
                    Message = validationResult.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }).ToArray()
                });
            }

            var newTask = dto.Adapt<NewTaskDto, TaskModel>();
            newTask.Status = TaskModelStatus.NotCompleted;

            await _context.Tasks.AddAsync(newTask, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(new ResponseMessageDto<TaskDto>(ResponseStatus.Ok)
            {
                Message = newTask.Adapt<TaskModel, TaskDto>()
            });
        }

        [Authorize]
        [HttpPost("edit/{id}")]
        public async Task<ActionResult<ResponseMessageDto<object>>> Edit(string text, TaskModelStatus status, int id, CancellationToken cancellationToken = default)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (task == null)
            {
                _logger.LogWarning("Задача c id=\"{id}\" не найдена.", id);

                return NotFound((new ResponseMessageDto<object>(ResponseStatus.Error)
                {
                    Message = $"Задача c id=\"{id}\" не найдена."
                }));
            }

            if (string.IsNullOrEmpty(text))
            {
                return BadRequest(new ResponseMessageDto<object>(ResponseStatus.Error)
                {
                    Message = new
                    {
                        Text = Constants.ERROR_MSG_REQUIRED_PROP
                    }
                });
            }

            task.Text = text;
            task.Status = status;

            await _context.SaveChangesAsync(cancellationToken);

            return Ok(new ResponseMessageDto<TaskDto>(ResponseStatus.Ok)
            {
                Message = task.Adapt<TaskModel, TaskDto>()
            });
        }
    }
}
