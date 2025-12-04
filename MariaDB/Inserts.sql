-- Active: 1709043130692@@127.0.0.1@3306@Biblioteca
INSERT INTO CatalogoLibro (Titulo, Autor, Anio, Descripcion) VALUES
('El Señor de los Anillos', 'J.R.R. Tolkien', 1954, 'Fantasía épica'),
('1984', 'George Orwell', 1949, 'Distopía política'),
('Cien años de soledad', 'Gabriel García Márquez', 1967, 'Realismo mágico'),
('Don Quijote', 'Miguel de Cervantes', 1605, 'Clásico de la literatura');

INSERT INTO Rol (Nombre) VALUES ('Admin'), ('Usuario');

UPDATE Usuario
SET RolId = 1
WHERE Id = 1;
