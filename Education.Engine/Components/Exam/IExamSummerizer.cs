using Education.Engine.Data.Exam;


namespace Education.Engine.Components.Exam
{
    interface IExamSummerizer
    {
        string GetSummary(ExamProcessContext processContext, ExamInfo examInfo);
    }
}
