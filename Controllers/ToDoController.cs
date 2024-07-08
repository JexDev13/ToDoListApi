using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApi.Models;

namespace ToDoListApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoListContext _context;

        public ToDoController(ToDoListContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las tareas.
        /// </summary>
        /// <returns>Lista de tareas.</returns>
        /// <response code="200">Devuelve la lista de todas las tareas.</response>
        /// <response code="401">Si no estás autorizado para acceder a las tareas.</response>
        [HttpGet]
        [Route("tasks")]
        public async Task<ActionResult<IEnumerable<TaskToDo>>> GetTasks()
        {
            return await _context.Tasks.Include(t => t.Comments).ToListAsync();
        }

        /// <summary>
        /// Obtiene una tarea por su ID.
        /// </summary>
        /// <param name="id">El ID de la tarea.</param>
        /// <returns>La tarea solicitada.</returns>
        /// <response code="200">Devuelve la tarea solicitada.</response>
        /// <response code="401">Si no estás autorizado para acceder a la tarea.</response>
        /// <response code="404">Si la tarea no existe.</response>
        [HttpGet]
        [Route("tasks/{id}")]
        public async Task<ActionResult<TaskToDo>> GetTask(int id)
        {
            var task = await _context.Tasks.Include(t => t.Comments).FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        /// <summary>
        /// Crea una nueva tarea.
        /// </summary>
        /// <param name="task">La tarea a crear.</param>
        /// <returns>La tarea creada.</returns>
        /// <response code="200">Devuelve la tarea creada.</response>
        /// <response code="400">Si la solicitud es inválida o los datos no son válidos.</response>
        /// <response code="401">Si no estás autorizado para crear una tarea.</response>
        /// <response code="500">Si ocurre un error interno del servidor al crear la tarea.</response>
        [HttpPost]
        [Route("tasks")]
        public async Task<ActionResult<TaskToDo>> PostTask(TaskToDo task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }


        /// <summary>
        /// Actualiza una tarea existente.
        /// </summary>
        /// <param name="id">El ID de la tarea.</param>
        /// <param name="task">La tarea actualizada.</param>
        /// <returns>NoContent si la actualización fue exitosa.</returns>
        [HttpPut]
        [Route("tasks/{id}")]
        public async Task<IActionResult> PutTask(int id, TaskToDo task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina una tarea por su ID.
        /// </summary>
        /// <param name="id">El ID de la tarea.</param>
        /// <returns>NoContent si la eliminación fue exitosa.</returns>
        /// <response code="204">NoContent si la tarea fue eliminada exitosamente.</response>
        /// <response code="401">Si no estás autorizado para eliminar la tarea.</response>
        /// <response code="404">Si la tarea no existe.</response>
        [HttpDelete]
        [Route("tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Obtiene los comentarios de una tarea.
        /// </summary>
        /// <param name="taskId">El ID de la tarea.</param>
        /// <returns>Lista de comentarios.</returns>
        /// <response code="200">Devuelve la lista de comentarios de la tarea.</response>
        /// <response code="401">Si no estás autorizado para acceder a los comentarios.</response>
        /// <response code="404">Si la tarea no existe.</response>
        [HttpGet]
        [Route("tasks/{taskId}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(int taskId)
        {
            var task = await _context.Tasks.Include(t => t.Comments).FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return NotFound();
            }

            var comments = task.Comments.Select(c => new Comment
            {
                Id = c.Id,
                Text = c.Text,
                IsUpdated = c.IsUpdated,
                TaskToDoId = c.TaskToDoId,
                ParentCommentId = c.ParentCommentId,
                Replies = c.Replies 
            }).ToList();

            return Ok(task.Comments);
        }

        /// <summary>
        /// Agrega un comentario a una tarea.
        /// </summary>
        /// <param name="taskId">El ID de la tarea.</param>
        /// <param name="comment">El comentario a agregar.</param>
        /// <returns>El comentario agregado.</returns>
        /// <response code="201">Creado si el comentario fue agregado exitosamente.</response>
        /// <response code="400">Si la solicitud es inválida o los datos no son válidos.</response>
        /// <response code="401">Si no estás autorizado para agregar un comentario.</response>
        [HttpPost]
        [Route("tasks/{taskId}/comments")]
        public async Task<ActionResult<Comment>> PostComment(int taskId, Comment comment)
        {
            // Verifica si la tarea existe
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound("La tarea no existe.");
            }

            // Asigna el taskId al comentario
            comment.TaskToDoId = taskId;

            // Asegúrate de que ParentCommentId sea null si no está especificado
            if (comment.ParentCommentId == 0)
            {
                comment.ParentCommentId = null;
            }

            // Agrega el comentario al contexto y guarda los cambios
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Devuelve el comentario recién creado
            return CreatedAtAction(nameof(GetTask), new { taskId, commentId = comment.Id }, comment);
        }


        /// <summary>
        /// Actualiza un comentario.
        /// </summary>
        /// <param name="taskId">El ID de la tarea.</param>
        /// <param name="commentId">El ID del comentario.</param>
        /// <param name="comment">El comentario actualizado.</param>
        /// <returns>NoContent si la actualización fue exitosa.</returns>
        /// <response code="204">NoContent si el comentario fue actualizado exitosamente.</response>
        /// <response code="400">Si la solicitud es inválida o los datos no son válidos.</response>
        /// <response code="401">Si no estás autorizado para actualizar el comentario.</response>
        /// <response code="404">Si la tarea o el comentario no existen.</response>
        [HttpPut]
        [Route("tasks/{taskId}/comments/{commentId}")]
        public async Task<IActionResult> PutComment(int taskId, int commentId, Comment comment)
        {
            if (commentId != comment.Id)
            {
                return BadRequest();
            }

            // Asegúrate de que la relación de TaskToDo esté cargada
            var existingComment = await _context.Comments
                .Include(c => c.TaskToDo)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (existingComment == null)
            {
                return NotFound("El comentario no existe.");
            }

            // Actualiza solo los campos necesarios
            existingComment.Text = comment.Text;
            existingComment.IsUpdated = comment.IsUpdated;

            _context.Entry(existingComment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina un comentario.
        /// </summary>
        /// <param name="taskId">El ID de la tarea.</param>
        /// <param name="commentId">El ID del comentario.</param>
        /// <returns>NoContent si la eliminación fue exitosa.</returns>
        /// <response code="204">NoContent si el comentario fue eliminado exitosamente.</response>
        /// <response code="401">Si no estás autorizado para eliminar el comentario.</response>
        /// <response code="404">Si la tarea o el comentario no existen.</response>
        [HttpDelete]
        [Route("tasks/{taskId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int taskId, int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
