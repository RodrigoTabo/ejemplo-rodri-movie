# Plataforma de Peliculas

Aplicación web de películas donde podés explorar un catálogo, filtrar por género y ver el detalle de cada título (sinopsis, duración, fecha de lanzamiento, plataforma y género).  
Los usuarios registrados pueden crear reseñas y asignar puntuaciones. Incluye roles y un panel de administración para gestionar contenido.

---

## Características

### Catálogo de películas
- Home con listado de películas
- Filtro por género
- Vista detalle con:
  - portada + título
  - sinopsis
  - duración
  - fecha de lanzamiento
  - plataforma disponible
  - género

### Reseñas y puntuaciones (requiere login)
- Crear reseña para una película
- Asignar puntuación
- Ver **mis reseñas**
- Ver listado general de reseñas realizadas
- Editar/actualizar reseñas

### Autenticación y perfil
- Registro e inicio de sesión
- Edición de usuario
- Foto de perfil

### Administración (rol Admin)
- ABM de películas
- ABM de géneros
- ABM de plataformas
- Moderación/edición de reseñas/comentarios de usuarios

---

## Tecnologías utilizadas
- **Backend:** ASP.NET Core (MVC / Razor Pages)
- **Base de datos:** SQL Server
- **ORM:** Entity Framework Core
- **Auth:** ASP.NET Core Identity (roles: User / Admin)
- **Frontend:** HTML, CSS, Bootstrap

---

## Cómo ejecutar el proyecto (local)

### Requisitos
- .NET SDK (10.0)
- SQL Server (LocalDB o instancia)
- (Opcional) Visual Studio / VS Code

### Instalación
1. Clonar el repositorio:
   ```bash
   git clone <URL_DEL_REPO>
   cd <CARPETA_DEL_PROYECTO>
