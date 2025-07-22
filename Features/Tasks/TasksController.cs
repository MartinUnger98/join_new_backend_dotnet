using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JoinBackendDotnet.Data;
using JoinBackendDotnet.Models;
using JoinBackendDotnet.DTOs;
using ModelsTask = JoinBackendDotnet.Models.Task;
using System.Runtime.Serialization;

namespace JoinBackendDotnet.Features.Tasks
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Subtasks)
                .Include(t => t.AssignedTo)
                .ToListAsync();

            return Ok(tasks.Select(MapToResponseDto));
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] TaskCreateDto dto)
        {
            if (!TryParseEnums(dto.Priority, dto.Category, dto.Status,
                out Priority priority, out Category category, out Status status, out string error))
                return BadRequest(error);

            if (!DateTime.TryParse(dto.DueDate, out var dueDate))
                return BadRequest("Invalid due_date format");

            var assignedContacts = await _context.Contacts
                .Where(c => dto.AssignedTo.Contains(c.Id))
                .ToListAsync();

            var task = new ModelsTask
            {
                Title = dto.Title ?? string.Empty,
                Description = dto.Description ?? string.Empty,
                DueDate = dueDate,
                Priority = priority,
                Category = category,
                Status = status,
                AssignedTo = assignedContacts,
                Subtasks = MapSubtasksFromCreateDto(dto.Subtasks)
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(MapToResponseDto(task));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto dto)
        {
            var task = await _context.Tasks
                .Include(t => t.Subtasks)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            if (!TryParseEnums(dto.Priority, dto.Category, dto.Status,
                out Priority priority, out Category category, out Status status, out string error))
                return BadRequest(error);

            if (!DateTime.TryParse(dto.DueDate, out var dueDate))
                return BadRequest("Invalid due_date format");

            var assignedContacts = await _context.Contacts
                .Where(c => dto.AssignedTo.Contains(c.Id))
                .ToListAsync();

            task.Title = dto.Title ?? string.Empty;
            task.Description = dto.Description ?? string.Empty;
            task.DueDate = dueDate;
            task.Priority = priority;
            task.Category = category;
            task.Status = status;
            task.AssignedTo = assignedContacts;

            _context.Subtasks.RemoveRange(task.Subtasks);
            task.Subtasks = MapSubtasksFromCreateDto(dto.Subtasks, task.Id);

            await _context.SaveChangesAsync();

            var response = new[]
            {
                new {
                    model = "join.task",
                    pk = task.Id,
                    fields = new {
                        title = task.Title,
                        description = task.Description,
                        due_date = task.DueDate.ToString("yyyy-MM-dd"),
                        priority = GetEnumMemberValue(task.Priority),
                        category = GetEnumMemberValue(task.Category),
                        status = GetEnumMemberValue(task.Status),
                        assigned_to = task.AssignedTo.Select(c => c.Id).ToList()
                    }
                }
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Subtasks)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            _context.Subtasks.RemoveRange(task.Subtasks);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Subtask Mapping
        private static List<Subtask> MapSubtasksFromCreateDto(IEnumerable<SubtaskCreateTaskDto> subtasks, int taskId = 0)
        {
            return subtasks.Select(s => new Subtask
            {
                Task = taskId,
                Value = s.Value ?? string.Empty,
                Edit = s.Edit,
                Done = false
            }).ToList();
        }

        // DTO Mapping
        private static TaskResponseDto MapToResponseDto(ModelsTask task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate.ToString("yyyy-MM-dd"),
                Priority = GetEnumMemberValue(task.Priority),
                Category = GetEnumMemberValue(task.Category),
                Status = GetEnumMemberValue(task.Status),
                AssignedTo = task.AssignedTo.Select(c => c.Id).ToList(),
                Subtasks = task.Subtasks.Select(st => new SubtaskDto
                {
                    Id = st.Id,
                    Task = st.Task,
                    Value = st.Value,
                    Edit = st.Edit,
                    Done = st.Done
                }).ToList()
            };
        }

        // Enum Parsing inkl. Fehlerausgabe
        private static bool TryParseEnums(string? priorityRaw, string? categoryRaw, string? statusRaw,
            out Priority priority, out Category category, out Status status, out string error)
        {
            priority = default!;
            category = default!;
            status = default!;
            error = string.Empty;

            if (!Enum.TryParse<Priority>(Normalize(priorityRaw), true, out priority))
            {
                error = $"Invalid priority value: {priorityRaw}";
                return false;
            }

            if (!Enum.TryParse<Category>(Normalize(categoryRaw), true, out category))
            {
                error = $"Invalid category value: {categoryRaw}";
                return false;
            }

            if (!Enum.TryParse<Status>(Normalize(statusRaw), true, out status))
            {
                error = $"Invalid status value: {statusRaw}";
                return false;
            }

            return true;
        }

        // EnumMember(Value="...") extrahieren
        private static string GetEnumMemberValue<T>(T enumValue) where T : Enum
        {
            var enumType = typeof(T);
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var attribute = memberInfo[0]
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .FirstOrDefault();

            return attribute?.Value ?? enumValue.ToString();
        }

        private static string Normalize(string? input)
            => input?.Replace(" ", "").Replace("_", "") ?? string.Empty;
    }
}
