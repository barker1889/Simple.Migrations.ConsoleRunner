using SimpleMigrations;

namespace SampleMigrations
{
    [Migration(1, "Adds customer table")]
    public class _001_CreateUsersTable : Migration
    {
        protected override void Up()
        {
            Execute(@"CREATE TABLE CUSTOMERS(
                           ID   INT              NOT NULL,
                           NAME VARCHAR (20)     NOT NULL,
                           AGE  INT              NOT NULL,
                           ADDRESS  CHAR (25) ,
                           SALARY   DECIMAL (18, 2),       
                           PRIMARY KEY (ID)
                        );");
        }

        protected override void Down()
        {
            Execute("DROP TABLE CUSTOMERS");
        }
    }
}
