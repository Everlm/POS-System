using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace POS.Application.Documents.Static
{
    public static class PdfStyles
    {
        public static TextStyle Title => TextStyle.Default.FontSize(20).Bold().FontColor(Colors.Blue.Medium);
        public static TextStyle Header => TextStyle.Default.FontSize(14).Bold();
        public static TextStyle Body => TextStyle.Default.FontSize(12);
        public static TextStyle Small => TextStyle.Default.FontSize(10);

    }



}