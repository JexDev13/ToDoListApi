# ToDoList API

Esta es una API para administrar una lista de tareas y comentarios asociados, utilizando MySQL como base de datos.

## Descripción

La API permite realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre tareas y sus comentarios asociados. Las principales funcionalidades incluyen:

- Crear, obtener, actualizar y eliminar tareas.
- Agregar, actualizar y eliminar comentarios en una tarea específica.

## Configuración del Entorno de Desarrollo

Para configurar el entorno de desarrollo y ejecutar la API, sigue estos pasos:

1. **Requisitos Previos:**
   - Instalar [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0).
   - Instalar [Visual Studio 2022](https://visualstudio.microsoft.com/vs/).

2. **Clonar el Repositorio:**
   ```bash
   git clone https://github.com/JexDev13/ToDoListApi.git
   cd ToDoListApi
   ```
   
3. **Configuración de la Base de Datos:**

Asegúrate de tener un servidor MySQL en ejecución.
Actualiza la cadena de conexión en appsettings.json con los detalles de tu base de datos MySQL:
json
```
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=todolistdb;user=tuUser;password=tuPassword"
  }
}
```
4. ***Ejecutar la Aplicación:***

Abre el proyecto en Visual Studio.
Configura el proyecto de inicio para el proyecto ToDoListApi.
Presiona F5 para compilar y ejecutar la API.

## Endpoints Disponibles
### Tareas
* GET /api/tasks: Obtiene todas las tareas.
* GET /api/tasks/{id}: Obtiene una tarea por su ID.
* POST /api/tasks: Crea una nueva tarea.
* PUT /api/tasks/{id}: Actualiza una tarea existente.
* DELETE /api/tasks/{id}: Elimina una tarea por su ID.

### Comentarios
* GET /api/tasks/{taskId}/comments: Obtiene los comentarios de una tarea.
* POST /api/tasks/{taskId}/comments: Agrega un comentario a una tarea.
* PUT /api/tasks/{taskId}/comments/{commentId}: Actualiza un comentario existente.
* DELETE /api/tasks/{taskId}/comments/{commentId}: Elimina un comentario.

