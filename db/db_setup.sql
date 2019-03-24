CREATE DATABASE Team_Concept_Local;
GO

USE Team_Concept_Local;
GO

CREATE TABLE [AspNetUsers] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(256) NOT NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [Admin] bit NOT NULL,
    [RefreshToken] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Skills] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Skills] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [NotificationContexts] (
    [Id] int NOT NULL IDENTITY,
    [Data] nvarchar(max) NOT NULL,
    [Time] datetime2 NOT NULL,
    [CreatedById] int NOT NULL,
    [NotificationType] int NOT NULL,
    [EntreatyId] int NULL,
    CONSTRAINT [PK_NotificationContexts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_NotificationContexts_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Notifications] (
    [Id] int NOT NULL IDENTITY,
    [Read] bit NOT NULL,
    [ReceiverId] int NOT NULL,
    [NotificationContextId] int NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notifications_NotificationContexts_NotificationContextId] FOREIGN KEY ([NotificationContextId]) REFERENCES [NotificationContexts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Notifications_AspNetUsers_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Students] (
    [Id] int NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [GroupAdmin] bit NOT NULL,
    [Program] int NOT NULL,
    [Role] nvarchar(max) NULL,
    [Picture] nvarchar(max) NULL,
    [Campus] int NOT NULL,
    [Description] nvarchar(max) NULL,
    [LinkedInLink] nvarchar(max) NULL,
    [PortfolioLink] nvarchar(max) NULL,
    [GroupId] int NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Students_AspNetUsers_Id] FOREIGN KEY ([Id]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [StudentsSkills] (
    [StudentId] int NOT NULL,
    [SkillId] int NOT NULL,
    CONSTRAINT [PK_StudentsSkills] PRIMARY KEY ([StudentId], [SkillId]),
    CONSTRAINT [FK_StudentsSkills_Skills_SkillId] FOREIGN KEY ([SkillId]) REFERENCES [Skills] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_StudentsSkills_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Entreaties] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(max) NOT NULL,
    [Accepted] bit NOT NULL,
    [StudentId] int NOT NULL,
    [GroupId] int NOT NULL,
    [EntreatyType] int NOT NULL,
    CONSTRAINT [PK_Entreaties] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Entreaties_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [GroupsProjects] (
    [GroupId] int NOT NULL,
    [ProjectId] int NOT NULL,
    [Preference] int NOT NULL,
    CONSTRAINT [PK_GroupsProjects] PRIMARY KEY ([GroupId], [ProjectId])
);

GO

CREATE TABLE [Projects] (
    [Id] int NOT NULL IDENTITY,
    [ProjectName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Difficulty] int NOT NULL,
    [IPType] int NOT NULL,
    [Approved] bit NOT NULL,
    [Proposed] bit NOT NULL,
    [ClientName] nvarchar(max) NOT NULL,
    [ClientEmail] nvarchar(max) NOT NULL,
    [ClientContact] nvarchar(max) NOT NULL,
    [Comments] nvarchar(max) NULL,
    [AssignedGroupId] int NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Groups] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Picture] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [SCMLink] nvarchar(max) NULL,
    [ProposedProjectId] int NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Groups_Projects_ProposedProjectId] FOREIGN KEY ([ProposedProjectId]) REFERENCES [Projects] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_AspNetUsers_UserName] ON [AspNetUsers] ([UserName]);

GO

CREATE INDEX [IX_Entreaties_GroupId] ON [Entreaties] ([GroupId]);

GO

CREATE UNIQUE INDEX [IX_Entreaties_StudentId_GroupId] ON [Entreaties] ([StudentId], [GroupId]);

GO

CREATE INDEX [IX_Groups_ProposedProjectId] ON [Groups] ([ProposedProjectId]);

GO

CREATE INDEX [IX_GroupsProjects_ProjectId] ON [GroupsProjects] ([ProjectId]);

GO

CREATE INDEX [IX_NotificationContexts_CreatedById] ON [NotificationContexts] ([CreatedById]);

GO

CREATE UNIQUE INDEX [IX_NotificationContexts_EntreatyId] ON [NotificationContexts] ([EntreatyId]) WHERE [EntreatyId] IS NOT NULL;

GO

CREATE INDEX [IX_Notifications_NotificationContextId] ON [Notifications] ([NotificationContextId]);

GO

CREATE INDEX [IX_Notifications_ReceiverId] ON [Notifications] ([ReceiverId]);

GO

CREATE UNIQUE INDEX [IX_Projects_AssignedGroupId] ON [Projects] ([AssignedGroupId]) WHERE [AssignedGroupId] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_Skills_Name] ON [Skills] ([Name]);

GO

CREATE INDEX [IX_Students_GroupId] ON [Students] ([GroupId]);

GO

CREATE INDEX [IX_StudentsSkills_SkillId] ON [StudentsSkills] ([SkillId]);

GO

ALTER TABLE [NotificationContexts] ADD CONSTRAINT [FK_NotificationContexts_Entreaties_EntreatyId] FOREIGN KEY ([EntreatyId]) REFERENCES [Entreaties] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [Students] ADD CONSTRAINT [FK_Students_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE SET NULL;

GO

ALTER TABLE [Entreaties] ADD CONSTRAINT [FK_Entreaties_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [GroupsProjects] ADD CONSTRAINT [FK_GroupsProjects_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [GroupsProjects] ADD CONSTRAINT [FK_GroupsProjects_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [Projects] ADD CONSTRAINT [FK_Projects_Groups_AssignedGroupId] FOREIGN KEY ([AssignedGroupId]) REFERENCES [Groups] ([Id]) ON DELETE SET NULL;

GO

