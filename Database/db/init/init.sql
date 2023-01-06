-- Project Name : noname
-- Date/Time    : 2023/01/06 15:29:44
-- Author       : crossover
-- RDBMS Type   : PostgreSQL
-- Application  : A5:SQL Mk-2

/*
  << 注意！！ >>
  BackupToTempTable, RestoreFromTempTable疑似命令が付加されています。
  これにより、drop table, create table 後もデータが残ります。
  この機能は一時的に $$TableName のような一時テーブルを作成します。
  この機能は A5:SQL Mk-2でのみ有効であることに注意してください。
*/

-- AspNetRoleClaims
--* BackupToTempTable
DROP TABLE if exists "AspNetRoleClaims" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "AspNetRoleClaims" (
  "Id" integer NOT NULL
  , "RoleId" text NOT NULL
  , "ClaimType" text
  , "ClaimValue" text
  , CONSTRAINT "AspNetRoleClaims_PKC" PRIMARY KEY ("Id")
) ;

CREATE INDEX "IX_AspNetRoleClaims_RoleId"
  ON "AspNetRoleClaims"("RoleId");

-- AspNetRoles
--* BackupToTempTable
DROP TABLE if exists "AspNetRoles" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "AspNetRoles" (
  "Id" text NOT NULL
  , "Name" character varying(256)
  , "NormalizedName" character varying(256)
  , "ConcurrencyStamp" text
  , CONSTRAINT "AspNetRoles_PKC" PRIMARY KEY ("Id")
) ;

CREATE UNIQUE INDEX "RoleNameIndex"
  ON "AspNetRoles"("NormalizedName");

-- AspNetUserClaims
--* BackupToTempTable
DROP TABLE if exists "AspNetUserClaims" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "AspNetUserClaims" (
  "Id" integer NOT NULL
  , "UserId" text NOT NULL
  , "ClaimType" text
  , "ClaimValue" text
  , CONSTRAINT "AspNetUserClaims_PKC" PRIMARY KEY ("Id")
) ;

CREATE INDEX "IX_AspNetUserClaims_UserId"
  ON "AspNetUserClaims"("UserId");

-- AspNetUserLogins
--* BackupToTempTable
DROP TABLE if exists "AspNetUserLogins" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "AspNetUserLogins" (
  "LoginProvider" text NOT NULL
  , "ProviderKey" text NOT NULL
  , "ProviderDisplayName" text
  , "UserId" text NOT NULL
  , CONSTRAINT "AspNetUserLogins_PKC" PRIMARY KEY ("LoginProvider","ProviderKey")
) ;

CREATE INDEX "IX_AspNetUserLogins_UserId"
  ON "AspNetUserLogins"("UserId");

-- AspNetUserRoles
--* BackupToTempTable
DROP TABLE if exists "AspNetUserRoles" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "AspNetUserRoles" (
  "UserId" text NOT NULL
  , "RoleId" text NOT NULL
  , CONSTRAINT "AspNetUserRoles_PKC" PRIMARY KEY ("UserId","RoleId")
) ;

CREATE INDEX "IX_AspNetUserRoles_RoleId"
  ON "AspNetUserRoles"("RoleId");

-- AspNetUsers
--* BackupToTempTable
DROP TABLE if exists "AspNetUsers" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "AspNetUsers" (
  "Id" text NOT NULL
  , "UserName" character varying(256)
  , "NormalizedUserName" character varying(256)
  , "Email" character varying(256)
  , "NormalizedEmail" character varying(256)
  , "EmailConfirmed" boolean NOT NULL
  , "PasswordHash" text
  , "SecurityStamp" text
  , "ConcurrencyStamp" text
  , "PhoneNumber" text
  , "PhoneNumberConfirmed" boolean NOT NULL
  , "TwoFactorEnabled" boolean NOT NULL
  , "LockoutEnd" timestamp(6) with time zone
  , "LockoutEnabled" boolean NOT NULL
  , "AccessFailedCount" integer NOT NULL
  , CONSTRAINT "AspNetUsers_PKC" PRIMARY KEY ("Id")
) ;

CREATE INDEX "EmailIndex"
  ON "AspNetUsers"("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex"
  ON "AspNetUsers"("NormalizedUserName");

-- AspNetUserTokens
--* BackupToTempTable
DROP TABLE if exists "AspNetUserTokens" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "AspNetUserTokens" (
  "UserId" text NOT NULL
  , "LoginProvider" text NOT NULL
  , "Name" text NOT NULL
  , "Value" text
  , CONSTRAINT "AspNetUserTokens_PKC" PRIMARY KEY ("UserId","LoginProvider","Name")
) ;

-- CartLine
--* BackupToTempTable
DROP TABLE if exists "CartLine" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "CartLine" (
  "CartLineID" integer NOT NULL
  , "ProductID" bigint NOT NULL
  , "Quantity" integer NOT NULL
  , "OrderID" integer
  , CONSTRAINT "CartLine_PKC" PRIMARY KEY ("CartLineID")
) ;

CREATE INDEX "IX_CartLine_OrderID"
  ON "CartLine"("OrderID");

CREATE INDEX "IX_CartLine_ProductID"
  ON "CartLine"("ProductID");

-- OrderDetails
--* BackupToTempTable
DROP TABLE if exists "OrderDetails" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "OrderDetails" (
  "Id" integer NOT NULL
  , "OrderID" integer NOT NULL
  , "ProductID" bigint NOT NULL
  , CONSTRAINT "OrderDetails_PKC" PRIMARY KEY ("Id")
) ;

-- Orders
--* BackupToTempTable
DROP TABLE if exists "Orders" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "Orders" (
  "OrderID" integer NOT NULL
  , "Name" text NOT NULL
  , "Line1" text NOT NULL
  , "Line2" text
  , "Line3" text
  , "City" text NOT NULL
  , "State" text NOT NULL
  , "Zip" text
  , "Country" text NOT NULL
  , "GiftWrap" boolean NOT NULL
  , "Shipped" boolean NOT NULL
  , CONSTRAINT "Orders_PKC" PRIMARY KEY ("OrderID")
) ;

-- Products
--* BackupToTempTable
DROP TABLE if exists "Products" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "Products" (
  "ProductID" bigint NOT NULL
  , "Name" text NOT NULL
  , "Description" text NOT NULL
  , "Price" numeric(8, 2) NOT NULL
  , "Category" text NOT NULL
  , CONSTRAINT "Products_PKC" PRIMARY KEY ("ProductID")
) ;

COMMENT ON TABLE "AspNetRoleClaims" IS 'AspNetRoleClaims';
COMMENT ON COLUMN "AspNetRoleClaims"."Id" IS 'Id';
COMMENT ON COLUMN "AspNetRoleClaims"."RoleId" IS 'RoleId';
COMMENT ON COLUMN "AspNetRoleClaims"."ClaimType" IS 'ClaimType';
COMMENT ON COLUMN "AspNetRoleClaims"."ClaimValue" IS 'ClaimValue';

COMMENT ON TABLE "AspNetRoles" IS 'AspNetRoles';
COMMENT ON COLUMN "AspNetRoles"."Id" IS 'Id';
COMMENT ON COLUMN "AspNetRoles"."Name" IS 'Name';
COMMENT ON COLUMN "AspNetRoles"."NormalizedName" IS 'NormalizedName';
COMMENT ON COLUMN "AspNetRoles"."ConcurrencyStamp" IS 'ConcurrencyStamp';

COMMENT ON TABLE "AspNetUserClaims" IS 'AspNetUserClaims';
COMMENT ON COLUMN "AspNetUserClaims"."Id" IS 'Id';
COMMENT ON COLUMN "AspNetUserClaims"."UserId" IS 'UserId';
COMMENT ON COLUMN "AspNetUserClaims"."ClaimType" IS 'ClaimType';
COMMENT ON COLUMN "AspNetUserClaims"."ClaimValue" IS 'ClaimValue';

COMMENT ON TABLE "AspNetUserLogins" IS 'AspNetUserLogins';
COMMENT ON COLUMN "AspNetUserLogins"."LoginProvider" IS 'LoginProvider';
COMMENT ON COLUMN "AspNetUserLogins"."ProviderKey" IS 'ProviderKey';
COMMENT ON COLUMN "AspNetUserLogins"."ProviderDisplayName" IS 'ProviderDisplayName';
COMMENT ON COLUMN "AspNetUserLogins"."UserId" IS 'UserId';

COMMENT ON TABLE "AspNetUserRoles" IS 'AspNetUserRoles';
COMMENT ON COLUMN "AspNetUserRoles"."UserId" IS 'UserId';
COMMENT ON COLUMN "AspNetUserRoles"."RoleId" IS 'RoleId';

COMMENT ON TABLE "AspNetUsers" IS 'AspNetUsers';
COMMENT ON COLUMN "AspNetUsers"."Id" IS 'Id';
COMMENT ON COLUMN "AspNetUsers"."UserName" IS 'UserName';
COMMENT ON COLUMN "AspNetUsers"."NormalizedUserName" IS 'NormalizedUserName';
COMMENT ON COLUMN "AspNetUsers"."Email" IS 'Email';
COMMENT ON COLUMN "AspNetUsers"."NormalizedEmail" IS 'NormalizedEmail';
COMMENT ON COLUMN "AspNetUsers"."EmailConfirmed" IS 'EmailConfirmed';
COMMENT ON COLUMN "AspNetUsers"."PasswordHash" IS 'PasswordHash';
COMMENT ON COLUMN "AspNetUsers"."SecurityStamp" IS 'SecurityStamp';
COMMENT ON COLUMN "AspNetUsers"."ConcurrencyStamp" IS 'ConcurrencyStamp';
COMMENT ON COLUMN "AspNetUsers"."PhoneNumber" IS 'PhoneNumber';
COMMENT ON COLUMN "AspNetUsers"."PhoneNumberConfirmed" IS 'PhoneNumberConfirmed';
COMMENT ON COLUMN "AspNetUsers"."TwoFactorEnabled" IS 'TwoFactorEnabled';
COMMENT ON COLUMN "AspNetUsers"."LockoutEnd" IS 'LockoutEnd';
COMMENT ON COLUMN "AspNetUsers"."LockoutEnabled" IS 'LockoutEnabled';
COMMENT ON COLUMN "AspNetUsers"."AccessFailedCount" IS 'AccessFailedCount';

COMMENT ON TABLE "AspNetUserTokens" IS 'AspNetUserTokens';
COMMENT ON COLUMN "AspNetUserTokens"."UserId" IS 'UserId';
COMMENT ON COLUMN "AspNetUserTokens"."LoginProvider" IS 'LoginProvider';
COMMENT ON COLUMN "AspNetUserTokens"."Name" IS 'Name';
COMMENT ON COLUMN "AspNetUserTokens"."Value" IS 'Value';

COMMENT ON TABLE "CartLine" IS 'CartLine';
COMMENT ON COLUMN "CartLine"."CartLineID" IS 'CartLineID';
COMMENT ON COLUMN "CartLine"."ProductID" IS 'ProductID';
COMMENT ON COLUMN "CartLine"."Quantity" IS 'Quantity';
COMMENT ON COLUMN "CartLine"."OrderID" IS 'OrderID';

COMMENT ON TABLE "OrderDetails" IS 'OrderDetails';
COMMENT ON COLUMN "OrderDetails"."Id" IS 'Id';
COMMENT ON COLUMN "OrderDetails"."OrderID" IS 'OrderID';
COMMENT ON COLUMN "OrderDetails"."ProductID" IS 'ProductID';

COMMENT ON TABLE "Orders" IS 'Orders';
COMMENT ON COLUMN "Orders"."OrderID" IS 'OrderID';
COMMENT ON COLUMN "Orders"."Name" IS 'Name';
COMMENT ON COLUMN "Orders"."Line1" IS 'Line1';
COMMENT ON COLUMN "Orders"."Line2" IS 'Line2';
COMMENT ON COLUMN "Orders"."Line3" IS 'Line3';
COMMENT ON COLUMN "Orders"."City" IS 'City';
COMMENT ON COLUMN "Orders"."State" IS 'State';
COMMENT ON COLUMN "Orders"."Zip" IS 'Zip';
COMMENT ON COLUMN "Orders"."Country" IS 'Country';
COMMENT ON COLUMN "Orders"."GiftWrap" IS 'GiftWrap';
COMMENT ON COLUMN "Orders"."Shipped" IS 'Shipped';

COMMENT ON TABLE "Products" IS 'Products';
COMMENT ON COLUMN "Products"."ProductID" IS 'ProductID';
COMMENT ON COLUMN "Products"."Name" IS 'Name';
COMMENT ON COLUMN "Products"."Description" IS 'Description';
COMMENT ON COLUMN "Products"."Price" IS 'Price';
COMMENT ON COLUMN "Products"."Category" IS 'Category';

