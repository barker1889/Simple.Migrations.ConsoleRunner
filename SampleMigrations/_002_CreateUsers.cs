using SimpleMigrations;

namespace SampleMigrations
{
    [Migration(2, "Adds users table")]
    public class _002_CreateUsers : Migration
    {
        protected override void Up()
        {
            Execute(@"CREATE TABLE USERS(
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
            Execute("DROP TABLE USERS");
        }
    }
}
