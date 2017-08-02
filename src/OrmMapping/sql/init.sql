use master
go

if exists (select * from sys.databases where name='TestDB')
drop database [TestDB]
go

create database [TestDB]
go

use [TestDB]
go

create table employees (
    id bigint identity(1,1) primary key,
    first_name varchar(255),
    last_name varchar(255),
    store_id bigint
)
go


create table salary (
    fee bigint,
	employee_id bigint,
    constraint fk_employee_id foreign key (employee_id) references employees (id),
);
go

create table stores (
    id bigint identity(11,1)  primary key,
    name varchar(255)
)
go

create table products (
    id bigint identity(101,1) primary key,
    name varchar(255),
    price dec(8,2)
)
go

create table store_product (
    id bigint identity(1001,1) primary key,
    store_id bigint,
    product_id bigint
)
go



