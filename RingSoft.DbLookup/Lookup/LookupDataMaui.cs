using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    public abstract class LookupDataMauiBase
    {
        public int PageSize { get; protected set; }

        public abstract int RowCount { get; }
        
        public LookupDefinitionBase LookupDefinition { get; }

        public event EventHandler LookupDataChanged;

        public LookupDataMauiBase(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        protected void FireLookupDataChangedEvent()
        {
            LookupDataChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract void GetInitData(int pageSize);

        public abstract string GetFormattedRowValue(int rowIndex, LookupColumnDefinitionBase column);

        public abstract string GetDatabaseRowValue(int rowIndex, LookupColumnDefinitionBase column);
    }
    public class LookupDataMaui<TEntity> : LookupDataMauiBase where TEntity : new()
    {
        public IQueryable<TEntity> BaseQuery { get; }

        public IQueryable<TEntity> ProcessedQuery { get; private set; }

        public List<TEntity> CurrentList { get; } = new List<TEntity>();

        public override int RowCount => CurrentList.Count;

        public LookupDataMaui(IQueryable<TEntity> query, LookupDefinitionBase lookupDefinition)
        : base(lookupDefinition)
        {
            BaseQuery = query;
        }

        public void ProcessLookup(IQueryable<TEntity> query, LookupDefinitionBase lookup)
        {
            var entity = query.FirstOrDefault();

            //var newQuery = ApplyWhereInt(query, "Product.Supplier.SupplierID");
            var newQuery = ApplyWhereDate(query, "OrderDate", DateTime.Parse("01/01/1998"));
            newQuery = ApplyOrder(newQuery, "OrderDate", "OrderBy");
            //var newQuery = ApplyWhereString(query, "Product.Supplier.CompanyName");
            //newQuery = ApplyOrder(query, "Product.Supplier.CompanyName", "OrderBy");

            foreach (var filter in lookup.FilterDefinition.UserFilters)
            {
                if (filter is FieldFilterDefinition fieldFilter 
                    && fieldFilter.FormulaToSearch.IsNullOrEmpty())
                {
                    //var newObject = GetObject(fieldFilter.FieldDefinition.PropertyName, fieldFilter);
                }
            }
        }

        private IQueryable<TEntity> ApplyWhereInt(IQueryable<TEntity> source, string property)
        {
            var newWhereMethod = GetWhereMethod();
            var param = Expression.Parameter(typeof(TEntity), "p");
            var returnExpression = GetPropertyExpression(property, param);

            var value = Expression.Constant(2);
            var body = Expression.Equal(returnExpression, value);
            var whereLambda = Expression.Lambda<Func<TEntity, bool>>(body, param);

            object whereResult = newWhereMethod
                .Invoke(null, new object[] { source, whereLambda });

            var whereQueryable = (IQueryable<TEntity>)whereResult;
            whereQueryable = whereQueryable.Take(5);

            return whereQueryable;
        }

        private IQueryable<TEntity> ApplyWhereDate(IQueryable<TEntity> source, string property, DateTime? date)
        {
            var newWhereMethod = GetWhereMethod();
            var param = Expression.Parameter(typeof(TEntity), "(p");
            var returnExpression = GetPropertyExpression(property, param);

            var value = Expression.Constant(date, typeof(DateTime?));
            var body = Expression.GreaterThan(returnExpression, value);
            
            var lessThanValue = Expression.Constant(DateTime.Parse("01/03/1998"), typeof(DateTime?));
            var lessBody = Expression.LessThan(returnExpression, lessThanValue);
            var andAlso = Expression.AndAlso(body, lessBody);

            var whereLambda = Expression.Lambda<Func<TEntity, bool>>(andAlso, param);

            object whereResult = newWhereMethod
                .Invoke(null, new object[] { source, whereLambda });
            var whereQueryable = (IQueryable<TEntity>)whereResult;

            //whereQueryable = whereQueryable.Take(5);

            return whereQueryable;
        }


        private static MethodInfo GetWhereMethod()
        {
            var methods = typeof(Queryable).GetMethods();
            var whereMethod = methods
                .Where(method => method.Name == "Where"
                                 && method.IsGenericMethodDefinition
                                 && method.GetGenericArguments().Length == 1
                                 && method.GetParameters().Length == 2)
                .FirstOrDefault();
            var newWhereMethod = whereMethod.MakeGenericMethod(typeof(TEntity));
            return newWhereMethod;
        }

        private IQueryable<TEntity> ApplyWhereString(IQueryable<TEntity> source, string property)
        {
            var newWhereMethod = GetWhereMethod();
            var param = Expression.Parameter(typeof(TEntity), "p");
            var returnExpression = GetPropertyExpression(property, param);

            var comparsionString = "C";
            Expression<Func<string>> idLambda = () => comparsionString;
            var CallMethod = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
            Expression callExpr = Expression.Call(returnExpression, CallMethod, idLambda.Body);
            Expression searchExpr = Expression.GreaterThanOrEqual(callExpr, Expression.Constant(0));

            Expression<Func<TEntity, bool>> myLambda =
                Expression.Lambda<Func<TEntity, bool>>(searchExpr, param);


            object whereResult = newWhereMethod
                .Invoke(null, new object[] { source, myLambda });


            var whereQueryable = (IQueryable<TEntity>)whereResult;
            whereQueryable = whereQueryable.Take(5);

            return whereQueryable;

        }

        private static Expression GetPropertyExpression(string property, ParameterExpression param)
        {
            var first = true;
            Expression returnExpression = null;
            var properties = property.Split('.');
            foreach (var newProperty in properties)
            {
                if (first)
                {
                    first = false;
                    returnExpression = Expression.Property(param, newProperty);
                }
                else
                {
                    returnExpression = Expression.Property(returnExpression, newProperty);
                }
            }

            return returnExpression;
        }

        static IQueryable<TEntity> ApplyOrder(IQueryable<TEntity> source, string property, string methodName)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrEmpty(property))
                return source;

            var lambda = GetLambda(property);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), GetType(property))
                .Invoke(null, new object[] { source, lambda });
            var queryAble = (IQueryable<TEntity>)result;
            queryAble = queryAble.Take(5);
            return queryAble;
        }

        private static LambdaExpression GetLambda(string property)
        {
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            string[] props = property.Split('.');
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            return lambda;
        }

        private static Type GetType(string property)
        {
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            string[] props = property.Split('.');
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            return type;

        }
        private object GetObject(string propertyName, FieldFilterDefinition fieldFilter)
        {
            if (fieldFilter.JoinDefinition == null)
            {
                return GblMethods.GetProperty<TEntity>(propertyName);
            }
            else
            {
                return GetObject(propertyName, fieldFilter.JoinDefinition);
            }
            return null;
        }

        private object GetObject(string propertyName, TableFieldJoinDefinition join, object parentObject = null)
        {
            object result = null;
            if (join.ParentObject == null)
            {
                result = GblMethods.GetProperty<TEntity>(join.ForeignKeyDefinition.ForeignObjectPropertyName);
            }
            else
            {
                if (join.ParentObject is LookupJoin parentJoin)
                {
                    if (parentJoin.ParentObject is LookupJoin lookupJoin)
                    {
                        var parent = GetObject(lookupJoin.JoinDefinition.ForeignKeyDefinition.ForeignObjectPropertyName, lookupJoin.JoinDefinition);
                        result = GblMethods.GetProperty(parent, join.ForeignKeyDefinition.ForeignObjectPropertyName);
                    }
                    else
                    {
                        result = GblMethods.GetProperty<TEntity>(propertyName);
                    }
                }
            }
            return result;
        }

        private Expression<Func<T, bool>> GetExpression<T>(T value, FilterItemDefinition filter)
            where T : struct
        {
            Func<int> func = () => 1;
            Expression<Func<int>> expression = Expression.Lambda<Func<int>>(Expression.Call(func.Method));
            return null;
        }

        public override void GetInitData(int pageSize)
        {
            PageSize = pageSize;

            var param = GblMethods.GetParameterExpression<TEntity>();
            var whereExpression = LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

            if (whereExpression == null)
            {
                ProcessedQuery = BaseQuery;
            }
            else
            {
                ProcessedQuery = FilterItemDefinition.FilterQuery(BaseQuery, param, whereExpression);
            }

            ProcessedQuery = ProcessedQuery.Take(PageSize);

            CurrentList.Clear();

            CurrentList.AddRange(ProcessedQuery);

            FireLookupDataChangedEvent();
        }

        public override string GetFormattedRowValue(int rowIndex, LookupColumnDefinitionBase column)
        {
            var row = CurrentList[rowIndex];
            return column.GetFormattedValue(row);
        }

        public override string GetDatabaseRowValue(int rowIndex, LookupColumnDefinitionBase column)
        {
            var row = CurrentList[rowIndex];
            return column.GetDatabaseValue(row);
        }
    }
}
