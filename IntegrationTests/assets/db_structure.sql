-- Script de criação do banco de dados
create database TestDb;

create table usuario (
	id int primary key,
	nome varchar(100) not null,
);