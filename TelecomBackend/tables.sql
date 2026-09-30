CREATE TABLE IF NOT EXISTS logs (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    trip_name varchar(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP 
);

CREATE TABLE IF NOT EXISTS log_points (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    trip_id INTEGER NOT NULL,
    measured_at_gps DATETIME NOT NULL,
    lat REAL NOT NULL,
    lon REAL NOT NULL,
    altitude REAL,
    speed REAL,
    mcc INTEGER,
    mnc INTEGER,
    cell_id BIGINT,
    dbm INTEGER,
    ta INTEGER,
    accuracy REAL,
    bearing REAL,
    measured_at_bts DATETIME,
    net_type varchar(50),

    FOREIGN KEY (trip_id) REFERENCES logs(id) ON DELETE CASCADE
);