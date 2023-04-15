-- Project Name : noname
-- Date/Time    : 2023/01/06 16:52:35
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

-- CartLine
--* BackupToTempTable
DROP TABLE if exists "CartLine" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "CartLine"
(
  "CartLineId" integer NOT NULL,
  "ProductId"  bigint  NOT NULL,
  "Quantity"   integer NOT NULL,
  "OrderId"    integer,
  CONSTRAINT "CartLine_PKC" PRIMARY KEY ("CartLineId")
);

CREATE INDEX "IX_CartLine_OrderId"
  ON "CartLine" ("OrderId");

CREATE INDEX "IX_CartLine_ProductId"
  ON "CartLine" ("ProductId");

-- OrderDetails
--* BackupToTempTable
DROP TABLE if exists "OrderDetails" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "OrderDetails"
(
  "Id"        integer NOT NULL,
  "OrderId"   integer NOT NULL,
  "ProductId" bigint  NOT NULL,
  CONSTRAINT "OrderDetails_PKC" PRIMARY KEY ("Id")
);

-- Orders
--* BackupToTempTable
DROP TABLE if exists "Orders" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "Orders"
(
  "OrderId"  integer NOT NULL,
  "Name"     text    NOT NULL,
  "Line1"    text    NOT NULL,
  "Line2"    text,
  "Line3"    text,
  "City"     text    NOT NULL,
  "State"    text    NOT NULL,
  "Zip"      text,
  "Country"  text    NOT NULL,
  "GiftWrap" boolean NOT NULL,
  "Shipped"  boolean NOT NULL,
  CONSTRAINT "Orders_PKC" PRIMARY KEY ("OrderId")
);

-- Products
--* BackupToTempTable
DROP TABLE if exists "Products" CASCADE;

--* RestoreFromTempTable
CREATE TABLE "Products"
(
  "ProductId"   bigint        NOT NULL,
  "Name"        text          NOT NULL,
  "Description" text          NOT NULL,
  "Price"       numeric(8, 2) NOT NULL,
  "Category"    text          NOT NULL,
  CONSTRAINT "Products_PKC" PRIMARY KEY ("ProductId")
);

COMMENT ON TABLE "CartLine" IS 'CartLine';
COMMENT ON COLUMN "CartLine"."CartLineId" IS 'CartLineId';
COMMENT ON COLUMN "CartLine"."ProductId" IS 'ProductId';
COMMENT ON COLUMN "CartLine"."Quantity" IS 'Quantity';
COMMENT ON COLUMN "CartLine"."OrderId" IS 'OrderId';

COMMENT ON TABLE "OrderDetails" IS 'OrderDetails';
COMMENT ON COLUMN "OrderDetails"."Id" IS 'Id';
COMMENT ON COLUMN "OrderDetails"."OrderId" IS 'OrderId';
COMMENT ON COLUMN "OrderDetails"."ProductId" IS 'ProductId';

COMMENT ON TABLE "Orders" IS 'Orders';
COMMENT ON COLUMN "Orders"."OrderId" IS 'OrderId';
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
COMMENT ON COLUMN "Products"."ProductId" IS 'ProductId';
COMMENT ON COLUMN "Products"."Name" IS 'Name';
COMMENT ON COLUMN "Products"."Description" IS 'Description';
COMMENT ON COLUMN "Products"."Price" IS 'Price';
COMMENT ON COLUMN "Products"."Category" IS 'Category';
