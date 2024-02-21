﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Role" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NOT NULL,
    "Description" text NULL,
    "Key" text NOT NULL,
    "CreatedOnUtc" timestamp without time zone NOT NULL,
    "LastUpdatedOnUtc" timestamp without time zone NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" integer NOT NULL,
    "LastUpdatedBy" integer NOT NULL,
    CONSTRAINT "PK_Role" PRIMARY KEY ("Id")
);

CREATE TABLE "UserType" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NOT NULL,
    "Description" text NULL,
    "Key" text NOT NULL,
    "CreatedOnUtc" timestamp without time zone NOT NULL,
    "LastUpdatedOnUtc" timestamp without time zone NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" integer NOT NULL,
    "LastUpdatedBy" integer NOT NULL,
    CONSTRAINT "PK_UserType" PRIMARY KEY ("Id")
);

CREATE TABLE "User" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "MobileNumber" text NOT NULL,
    "Email" text NOT NULL,
    "Address" text NOT NULL,
    "UserTypeId" integer NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedOnUtc" timestamp without time zone NOT NULL,
    "LastUpdatedOnUtc" timestamp without time zone NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" integer NOT NULL,
    "LastUpdatedBy" integer NOT NULL,
    CONSTRAINT "PK_User" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_User_UserType_UserTypeId" FOREIGN KEY ("UserTypeId") REFERENCES "UserType" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserRole" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" integer NOT NULL,
    "RoleId" integer NOT NULL,
    "CreatedOnUtc" timestamp without time zone NOT NULL,
    "LastUpdatedOnUtc" timestamp without time zone NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "CreatedBy" integer NOT NULL,
    "LastUpdatedBy" integer NOT NULL,
    CONSTRAINT "PK_UserRole" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserRole_Role_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRole_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_User_UserTypeId" ON "User" ("UserTypeId");

CREATE INDEX "IX_UserRole_RoleId" ON "UserRole" ("RoleId");

CREATE INDEX "IX_UserRole_UserId" ON "UserRole" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240221141356_initial_setup', '5.0.10');

COMMIT;

