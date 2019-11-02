I simplified the application so the Database has only 4 tables.

1).Table Test containts informations about Test.
2).Table Question contains informations about question
Every question can have 3 posible answers and only one answer can be correct.

3).Table TestResult contains information about test results as well as sessionId so that i can know
which user has attended the test.

4).Table Testing contains information about every question and every answer that user has answered,
as well the information if he answer correctly.SessionId and QuestionId is unique constraint becouse when user
begin test he can't answer on two same questions.


I did some insert querry for one test.

Also did't do Cascading so once you done the test you won't be able to delete it unless you delete data in other tables first.