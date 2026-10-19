CREATE TABLE supplier (
    id_supplier SERIAL PRIMARY KEY,
    organization_name VARCHAR(200) NOT NULL,
    contact_person VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    address VARCHAR(300) NOT NULL,
    email VARCHAR(100) NOT NULL
);
SELECT * FROM "user";

-- 1. Создаем таблицу ролей
CREATE TABLE role (
    id_role SERIAL PRIMARY KEY,
    name_role VARCHAR(100) UNIQUE NOT NULL
);

-- 2. Создаем таблицу пользователей
CREATE TABLE "user" (
    id_user SERIAL PRIMARY KEY,
    id_role INT NOT NULL,
    login_user VARCHAR(100) UNIQUE NOT NULL,
    password_user VARCHAR(255) NOT NULL,
    CONSTRAINT user_id_role_fkey FOREIGN KEY (id_role) REFERENCES role(id_role)
);

-- 3. Добавляем роль Администратора
INSERT INTO role (name_role) VALUES ('Администратор');

-- 4. Создаем тестового пользователя (логин: admin, пароль: 123)
INSERT INTO "user" (id_role, login_user, password_user) 
VALUES (1, 'admin', '123');

-- 5. Проверяем, что пользователь создался
SELECT * FROM "user";