USE [SCCDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertMessages]    Script Date: 4/25/2017 6:19:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[SP_InsertMessages](
			@Group  int=0,
            @ExtensionKey1 nvarchar(50)=null,
            @Type int=0,
            @Code  nvarchar(50)=null,
            @Content  nvarchar(max)=null,
            @ExtensionKey2  nvarchar(50)=null,
            @Tags  nvarchar(255)=null,
            @Method  nvarchar(255)=null,
            @StackTrace  nvarchar(max)=null,
			@User nvarchar(20)=null
)
as
INSERT INTO [dbo].[Messages]
           ([Group]
           ,[ExtensionKey1]
           ,[Type]
           ,[Code]
           ,[Content]
           ,[ExtensionKey2]
           ,[Tags]
           ,[Method]
           ,[StackTrace]
		   ,[User])
     VALUES
           (@Group 
           ,@ExtensionKey1 
           ,@Type 
           ,@Code 
           ,@Content 
           ,@ExtensionKey2 
           ,@Tags 
           ,@Method 
           ,@StackTrace
		   ,@User )
