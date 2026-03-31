--  Countries table to cache data from REST Countries API
CREATE TABLE Countries (
    Id         INT           IDENTITY(1,1) PRIMARY KEY,  
    Code       NVARCHAR(10)  NOT NULL UNIQUE,            
    Name       NVARCHAR(100) NOT NULL,                    
    Capital    NVARCHAR(100) NULL,                        
    Population BIGINT        NULL,                        
    Region     NVARCHAR(50)  NULL,                        
    CreatedAt  DATETIME2     DEFAULT GETDATE()            -- Local time
);