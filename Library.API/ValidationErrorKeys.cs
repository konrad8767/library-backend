namespace Library.API
{
    public static class ValidationErrorKeys
    {
        public static readonly string BookIdInvalid = "BOOK_ID_INVALID";
        public static readonly string FilterNotAllowed = "FILTER_NOT_ALLOWED";
        public static readonly string ConditionNotAllowed = "CONDITION_NOT_ALLOWED";
        public static readonly string BookTitleCannotContainOnlySpecialChars = "BOOK_TITLE_CANNOT_CONTAIN_ONLY_SPECIAL_CHARS";
        public static readonly string VersionMustBeGreaterThanZero = "VERSION_MUST_BE_GREATER_THAN_ZERO";
        public static readonly string PublicationDateCannotBeLaterThanCurrent = "PUBLICATION_DATE_CANNOT_BE_LATER_THAN_CURRENT";
        public static readonly string BookMustHaveAtLeastOneAuthor = "BOOK_MUST_HAVE_AT_LEAST_ONE_AUTHOS";
    }
}
