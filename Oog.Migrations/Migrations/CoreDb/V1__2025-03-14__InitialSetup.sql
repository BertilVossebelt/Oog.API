CREATE SCHEMA IF NOT EXISTS public;

-- Environment table
CREATE TABLE IF NOT EXISTS public.env (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

-- User table
CREATE TABLE IF NOT EXISTS public.account (
    id SERIAL PRIMARY KEY,                          
    username VARCHAR(255) UNIQUE NOT NULL,
    password TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Env_User table (many-to-many relationship)
CREATE TABLE IF NOT EXISTS public.env_account (
    account_id INT REFERENCES public.account(id) ON DELETE CASCADE,
    env_id INT REFERENCES public.env(id) ON DELETE CASCADE,
    owner BOOLEAN DEFAULT FALSE,
    maintainer BOOLEAN DEFAULT FALSE,
    PRIMARY KEY (account_id, env_id)
);

-- Application table
CREATE TABLE IF NOT EXISTS public.app (
    id SERIAL PRIMARY KEY,
    env_id INT REFERENCES public.env(id) ON DELETE CASCADE,
    name VARCHAR(255) NOT NULL,
    passkey TEXT NOT NULL
);

-- Role table
CREATE TABLE IF NOT EXISTS public.role (
    id SERIAL PRIMARY KEY,
    env_id INT REFERENCES public.env(id) ON DELETE CASCADE,
    name VARCHAR(255) NOT NULL
);

-- User_Role table (many-to-many relationship)
CREATE TABLE IF NOT EXISTS public.account_role (
    account_id INT REFERENCES public.account(id) ON DELETE CASCADE,
    role_id INT REFERENCES public.role(id) ON DELETE CASCADE,
    PRIMARY KEY (account_id, role_id)
);

-- Tag table
CREATE TABLE IF NOT EXISTS public.tag (
    id SERIAL PRIMARY KEY,
    env_id INT REFERENCES public.env(id) ON DELETE CASCADE,
    name VARCHAR(255) NOT NULL
);