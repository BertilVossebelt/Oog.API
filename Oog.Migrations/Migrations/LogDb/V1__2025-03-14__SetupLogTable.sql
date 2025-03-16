CREATE TABLE IF NOT EXISTS log
(
    id UUID DEFAULT generateUUIDv4(),   -- Unique log ID
    severity String,                    -- Log severity level (INFO, WARNING, ERROR, etc.)
    tags Array(String),                 -- Multiple tags for filtering
    log_details String,                 -- Full log message/details
    log_datetime DATETIME,              -- Log date 
    env_id UInt32,                      -- Customer identifier (mapped to PostgreSQL) (partitioning column)
    app_id UInt32                       -- Application identifier (mapped to PostgreSQL)
)
    
ENGINE = MergeTree()
PARTITION BY env_id
ORDER BY (env_id, app_id, log_datetime, severity)
SETTINGS index_granularity = 8192;