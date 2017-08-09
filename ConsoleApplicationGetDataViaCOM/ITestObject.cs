namespace ComApi
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Интерфейс COM-объекта, который будет виден другим приложениям.
    /// </summary>
    [ComVisible(true), Guid("9CF35A8E-D819-4A42-997F-B66520704E78"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ITestObject
    {
        /// <summary>
        /// Метод для быстрой проверки работоспособности COM объекта.
        /// </summary>
        string SayHello();
        /// <summary>
        /// Позволяет выбрать изделие для обработки.
        /// </summary>
        /// <returns>Идентификатор версии изделия или Intermech.Consts.UnknownObjectId, если пользователь отказался выбирать изделие</returns>
        long SelectArticle();
        /// <summary>
        /// Возвращает заголовки объектов, из которых состоит указанное изделие.
        /// </summary>
        /// <param name="articleObjectId">Идентификатор версии изделия</param>
        /// <returns>Массив заголовков объектов проектного состава изделия. Через COM-передается в виде SafeArray(BSTR)</returns>
        string[] GetArticleStructure(int articleObjectId);
        /// <summary>
        /// Возвращает проектный состав указанного изделия.
        /// </summary>
        /// <param name="articleObjectId">Идентификатор версии изделия</param>
        /// <param name="attributes">Имена атрибутов изделий, которые необходимо вернуть</param>
        /// <returns>Прямоугольная матрица с результатами. В каждой строке матрицы - информация об одном изделии в составе указанного изделия, в столбцах
        /// матрицы - значения атрибутов изделий. Через COM-передается в виде SafeArray(VARIANT)</returns>
        object[,] GetArticleStructure2(int articleObjectId, string[] attributes);
    }
}
