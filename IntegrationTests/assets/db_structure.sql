-- Script de criação do banco de dados
drop database if exists TestDb;
create database TestDb;

use TestDb;

create table usuario (
    id int primary key,
    nome varchar(100) not null,
);