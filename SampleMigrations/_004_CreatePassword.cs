using SimpleMigrations;

namespace SampleMigrations
{
    [Migration(4, "Adds accounts table")]
    public class _004_CreatePassword : Migration
    {
        protected override void Up()
        {
            Execute(@"CREATE TABLE ACCOUNTS(
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
            Execute("DROP TABLE ACCOUNTS");
        }
    }
}