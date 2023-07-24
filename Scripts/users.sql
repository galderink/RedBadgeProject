CREATE table [Users](
[Id] int not null PRIMARY KEY IDENTITY(1,1),
[FirstName] NvarChar(100) Not Null,
[LastName] NvarChar(100) Not Null,
[Address] nvarchar(250) not null,
[Password] Nvarchar(100) not null
[Username] NvarChar(100) not null,
)