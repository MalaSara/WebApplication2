CREATE DATABASE IF NOT EXISTS mydatabse;
CREATE USER IF NOT EXISTS admin IDENTIFIED WITH plaintext_password BY 'admin';
GRANT ALL ON mydatabase.* TO admin