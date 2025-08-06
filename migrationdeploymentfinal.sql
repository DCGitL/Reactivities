Use EventDb



BEGIN TRANSACTION;
CREATE TABLE "Activities" (
    "Id"   nvarchar(250) NOT NULL CONSTRAINT "PK_Activities" PRIMARY KEY,
    "Title" TEXT NOT NULL,
    "Date" DateTime NOT NULL,
    "Description" nvarchar(500) NOT NULL,
    "Category" nvarchar(500) NOT NULL,
    "IsCancelled" INTEGER NOT NULL,
    "City" nvarchar(500) NOT NULL,
    "Venue" nvarchar(500) NOT NULL,
    "Latitude" REAL NOT NULL,
    "Longitude" REAL NOT NULL
);

CREATE TABLE "AspNetRoles" (
    "Id"   nvarchar(250) NOT NULL CONSTRAINT "PK_AspNetRoles" PRIMARY KEY,
    "Name" TEXT NULL,
    "NormalizedName" varchar(200) NULL,
    "ConcurrencyStamp" TEXT NULL
);

CREATE TABLE "AspNetUsers" (
    "Id"  nvarchar(250) CONSTRAINT "PK_AspNetUsers" PRIMARY KEY,
    "DisplayName" nvarchar(200) NULL,
    "Bio" TEXT NULL,
    "ImageUrl" nvarchar(500) NULL,
    "UserName" nvarchar(256) NULL,
    "NormalizedUserName" nvarchar(256) NULL,
    "Email" nvarchar(256) NULL,
    "NormalizedEmail" nvarchar(256) NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" nvarchar(max) NULL,
    "SecurityStamp" nvarchar(max) NULL,
    "ConcurrencyStamp" nvarchar(max) NULL,
    "PhoneNumber" nvarchar(20) NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" datetimeoffset(7) NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);
CREATE TABLE "AspNetRoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY IDentity(1,1),
    "RoleId"  nvarchar(250) NOT NULL,
    "ClaimType" nvarchar(max) NULL,
    "ClaimValue" nvarchar(max) NULL,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ActivityAttendees" (
    "UserId"  nvarchar(250) NOT NULL,
    "ActivityId"   nvarchar(250) NOT NULL,
    "IsHost" INTEGER NOT NULL,
    "DateJoined" Datetime NOT NULL,
    CONSTRAINT "PK_ActivityAttendees" PRIMARY KEY ("ActivityId", "UserId"),
    CONSTRAINT "FK_ActivityAttendees_Activities_ActivityId" FOREIGN KEY ("ActivityId") REFERENCES "Activities" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ActivityAttendees_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY IDentity(1,1),
    "UserId"  nvarchar(250) NOT NULL,
    "ClaimType" nvarchar(max) NULL,
    "ClaimValue" nvarchar(max) NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" nvarchar(250) NOT NULL,
    "ProviderKey" nvarchar(250) NOT NULL,
    "ProviderDisplayName" nvarchar(max) NULL,
    "UserId"   nvarchar(250) NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId"   nvarchar(250) NOT NULL,
    "RoleId"   nvarchar(250) NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId"  nvarchar(250) NOT NULL,
    "LoginProvider" nvarchar(250) NOT NULL,
    "Name" nvarchar(250) NOT NULL,
    "Value" nvarchar(max) NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ActivityAttendees_UserId" ON "ActivityAttendees" ("UserId");

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");


CREATE TABLE "Photos" (
    "Id"  nvarchar(250) NOT NULL CONSTRAINT "PK_Photos" PRIMARY KEY,
    "Url" TEXT NOT NULL,
    "PublicId" nvarchar(250) NOT NULL,
    "UserId"  nvarchar(250) NOT NULL,
    CONSTRAINT "FK_Photos_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Photos_UserId" ON "Photos" ("UserId");



CREATE TABLE "Comments" (
    "Id"  nvarchar(250) NOT NULL CONSTRAINT "PK_Comments" PRIMARY KEY,
    "Body" TEXT NOT NULL,
    "CreatedAt" Datetime NOT NULL,
    "UserId"  nvarchar(250) NOT NULL,
    "ActivityId"  nvarchar(250) NOT NULL,
    CONSTRAINT "FK_Comments_Activities_ActivityId" FOREIGN KEY ("ActivityId") REFERENCES "Activities" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Comments_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserFollowings" (
    "ObserverId"  nvarchar(250) NOT NULL,
    "TargetId"  nvarchar(250) NOT NULL,
    CONSTRAINT "PK_UserFollowings" PRIMARY KEY ("ObserverId", "TargetId"),
    CONSTRAINT "FK_UserFollowings_AspNetUsers_ObserverId" FOREIGN KEY ("ObserverId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserFollowings_AspNetUsers_TargetId" FOREIGN KEY ("TargetId") REFERENCES "AspNetUsers" ("Id") ON DELETE NO ACTION
);

CREATE INDEX "IX_Comments_ActivityId" ON "Comments" ("ActivityId");

CREATE INDEX "IX_Comments_UserId" ON "Comments" ("UserId");

CREATE INDEX "IX_UserFollowings_TargetId" ON "UserFollowings" ("TargetId");



CREATE INDEX "IX_Activities_Date" ON "Activities" ("Date");



COMMIT;

