-- postgres-init/init.sql
-- postgres-init/init.sql
DROP TABLE IF EXISTS connectors CASCADE;
DROP TABLE IF EXISTS charge_stations CASCADE;
DROP TABLE IF EXISTS groups CASCADE;

CREATE TABLE IF NOT EXISTS groups (
    id UUID PRIMARY KEY,
    name VARCHAR(255),
    capacity INT NOT NULL CHECK (capacity > 0)
);

CREATE TABLE IF NOT EXISTS charge_stations (
    id UUID PRIMARY KEY,
    name VARCHAR(255),
    group_id UUID NOT NULL,
    FOREIGN KEY (group_id) REFERENCES groups(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS connectors (
    id INT NOT NULL,
    max_current INT NOT NULL CHECK (max_current > 0),
    charge_station_id UUID NOT NULL,
    PRIMARY KEY (charge_station_id, id), -- Composite primary key
    FOREIGN KEY (charge_station_id) REFERENCES charge_stations(id) ON DELETE CASCADE,
    CONSTRAINT charge_station_connector_id_range CHECK (id BETWEEN 1 AND 5)
);
