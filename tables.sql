CREATE TABLE logs (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    trip_name varchar(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP 
);

CREATE TABLE log_points (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    trip_id INTEGER NOT NULL,
    measured_at DATETIME NOT NULL,
    lat REAL NOT NULL,
    lon REAL NOT NULL,
    short_cell_id INTEGER,
    signal_strenght INTEGER, 
    speed REAL, 
    net_type varchar(50),

    FOREIGN KEY (trip_id) REFERENCES logs(id) ON CASCADE DELETE
);