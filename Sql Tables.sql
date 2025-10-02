CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(250) NOT NULL,
    Author NVARCHAR(200) NOT NULL,
    ISBN NVARCHAR(50) NOT NULL,
    PublishedYear INT NULL,
    AvailableCopies INT NOT NULL CONSTRAINT CK_Books_AvailableCopies CHECK (AvailableCopies >= 0),
    CONSTRAINT UQ_Books_ISBN UNIQUE (ISBN)
);

CREATE TABLE Members (
    MemberId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(50) NULL,
    JoinDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT UQ_Members_Email UNIQUE (Email)
);

CREATE TABLE BorrowRecords (
    BorrowId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    BookId INT NOT NULL,
    BorrowDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ReturnDate DATETIME2 NULL,
    IsReturned BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Borrow_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId),
    CONSTRAINT FK_Borrow_Books FOREIGN KEY (BookId) REFERENCES Books(BookId)
);


