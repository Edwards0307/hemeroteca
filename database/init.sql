CREATE TABLE IF NOT EXISTS Categorias (
    Id SERIAL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS Usuarios (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    FechaCreacion TIMESTAMP DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS Libros (
    Id SERIAL PRIMARY KEY,
    Codigo VARCHAR(50),
    Titulo VARCHAR(255) NOT NULL,
    Autor VARCHAR(255),
    Editorial VARCHAR(255),
    Idioma VARCHAR(50),
    Paginas INTEGER,
    Ano INTEGER,
    Descripcion TEXT,
    RutaImagen VARCHAR(500),
    RutaArchivo VARCHAR(500),
    TipoDocumento VARCHAR(50),
    FechaPublicacion TIMESTAMP,
    FechaRegistro TIMESTAMP DEFAULT NOW(),
    TotalDescargas INTEGER DEFAULT 0,
    CategoriaId INTEGER REFERENCES Categorias(Id)
);

CREATE TABLE IF NOT EXISTS Revistas (
    Id SERIAL PRIMARY KEY,
    Titulo VARCHAR(255) NOT NULL,
    Autor VARCHAR(255),
    LugarPublicacion VARCHAR(255),
    Idioma VARCHAR(50),
    Ano INTEGER,
    Descripcion TEXT,
    RutaArchivo VARCHAR(500),
    TipoDocumento VARCHAR(50),
    FechaPublicacion TIMESTAMP,
    FechaRegistro TIMESTAMP DEFAULT NOW(),
    TotalDescargas INTEGER DEFAULT 0,
    CategoriaId INTEGER REFERENCES Categorias(Id)
);

-- Datos de ejemplo
INSERT INTO Categorias (Nombre) VALUES
    ('Literatura'),
    ('Ciencia'),
    ('Historia'),
    ('Tecnología'),
    ('Arte y Cultura')
ON CONFLICT DO NOTHING;

INSERT INTO Libros (Codigo, Titulo, Autor, Editorial, Idioma, Paginas, Ano, Descripcion, TipoDocumento, CategoriaId) VALUES
    ('LIB-001', 'Cien Años de Soledad', 'Gabriel García Márquez', 'Editorial Sudamericana', 'Español', 471, 1967, 'La obra cumbre del realismo mágico latinoamericano.', 'PDF', 1),
    ('LIB-002', 'El Origen de las Especies', 'Charles Darwin', 'John Murray', 'Español', 502, 1859, 'La obra fundamental de la biología evolutiva moderna.', 'PDF', 2),
    ('LIB-003', 'Sapiens: De animales a dioses', 'Yuval Noah Harari', 'Debate', 'Español', 496, 2011, 'Una breve historia de la humanidad desde el homo sapiens hasta la actualidad.', 'PDF', 3),
    ('LIB-004', 'Clean Code', 'Robert C. Martin', 'Prentice Hall', 'Inglés', 431, 2008, 'Guía de principios para escribir código limpio y mantenible.', 'PDF', 4),
    ('LIB-005', 'Historia del Arte', 'Ernst Gombrich', 'Phaidon', 'Español', 688, 1950, 'El libro más vendido sobre la historia del arte en el mundo occidental.', 'PDF', 5)
ON CONFLICT DO NOTHING;

INSERT INTO Revistas (Titulo, Autor, LugarPublicacion, Idioma, Ano, Descripcion, TipoDocumento, CategoriaId) VALUES
    ('National Geographic - Edición Especial Amazonia', 'National Geographic Society', 'Washington D.C.', 'Español', 2023, 'Exploración completa del ecosistema amazónico y su biodiversidad.', 'PDF', 2),
    ('Revista de Historia y Cultura', 'Academia Colombiana de Historia', 'Bogotá', 'Español', 2022, 'Publicación académica sobre eventos históricos de América Latina.', 'PDF', 3),
    ('MIT Technology Review', 'MIT Press', 'Cambridge', 'Inglés', 2023, 'Las últimas innovaciones en inteligencia artificial y computación cuántica.', 'PDF', 4),
    ('El Malpensante', 'Revista El Malpensante', 'Bogotá', 'Español', 2023, 'Literatura, arte y cultura contemporánea iberoamericana.', 'PDF', 1)
ON CONFLICT DO NOTHING;
