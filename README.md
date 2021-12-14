# Ateo-API-Test

Please update the  connectionString in appsetting.js with Credentials of your Sql Server instance and the actual db name.

For the db to be created automatically with EF code first process , follow these steps :  
1. remove the migrations folder from the project if existed.  
2. Open Package Manager Console and type the following commands  :  
    1. add-migration "initial create"  
    2. update-database





If you wish to create the db manually : use the below sql script:   
To Create the db with name ateoTestDb:  
~~~~sql 
Create Database ateoTestDb;
~~~~

To create the persons table inside the aforementioned db :      
~~~~sql
use ateoTestDb 
Go

CREATE TABLE Persons (
    Id uniqueidentifier NOT NULL PRIMARY KEY,
    Nom varchar(50),
    Prenom varchar(50),
    Poste varchar(50)
);
~~~~

Once the db and Persons table are created , you may start testing the operations from the swagger interface or using postman