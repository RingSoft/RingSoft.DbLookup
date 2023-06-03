using System;
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
    public class LookupDataMaui<TEntity> where TEntity : class, new()
    {
        public void ProcessLookup(IQueryable<TEntity> query, LookupDefinitionBase lookup)
        {
            foreach (var filter in lookup.FilterDefinition.UserFilters)
            {
                if (filter is FieldFilterDefinition fieldFilter 
                    && fieldFilter.FormulaToSearch.IsNullOrEmpty())
                {
                    var newQuery = ApplyWhere(query, "ProductID", "OrderBy");
                    var result = newQuery.Take(5);
                    //var newObject = GetObject(fieldFilter.FieldDefinition.PropertyName, fieldFilter);
                }
            }
        }

        private IQueryable<TEntity> ApplyWhere(IQueryable<TEntity> source, string property, string methodName)
        {

            if (source == null)
                throw new ArgumentNullException("source");

            if (string.IsNullOrEmpty(property))
                return source;


            string[] props = property.Split('.');
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            var methods = typeof(Queryable).GetMethods();
            var whereMethods = methods.Where(method => method.Name == "Where");
            foreach (var whereMethod in whereMethods)
            {
                var genericArguments = whereMethod.GetGenericArguments();
                var parameters = whereMethod.GetParameters();
                if (parameters.Length == 2)
                {
                    var newWhereMethod = whereMethod.MakeGenericMethod(typeof(TEntity));
                    var param = Expression.Parameter(typeof(TEntity), "p");
                    
                    Expression bExpression = Expression.Property(param, "Product");
                    Expression numExpression = Expression.Property(bExpression, "Supplier");
                    //Expression thirdExpression = Expression.Property(numExpression, "SupplierID");
                    Expression thirdExpression = Expression.Property(numExpression, "CompanyName");

                    var whereProperty = thirdExpression;
                    //var value = Expression.Constant(2);
                    //var body = Expression.Equal(whereProperty, value);
                    //var whereLambda = Expression.Lambda<Func<TEntity, bool>>(body, param);

                    var comparsionString = "C";
                    Expression<Func<string>> idLambda = () => comparsionString;
                    var CallMethod = typeof(string).GetMethod("CompareTo", new[] { typeof(string) });
                    Expression callExpr = Expression.Call(thirdExpression, CallMethod, idLambda.Body);
                    Expression searchExpr = Expression.GreaterThanOrEqual(callExpr, Expression.Constant(0));

                    Expression<Func<TEntity, bool>> myLambda =
                        Expression.Lambda<Func<TEntity, bool>>(searchExpr, param);
                    

                    //object whereResult = newWhereMethod
                    //    .Invoke(null, new object[] { source, whereLambda });
                    object whereResult = newWhereMethod
                        .Invoke(null, new object[] {source, myLambda });


                    var whereQueryable = (IQueryable<TEntity>)whereResult;
                    whereQueryable = whereQueryable.Take(5);
                }
            }

            var orderByMethod = methods.Single(
                method => method.Name == methodName
                          && method.IsGenericMethodDefinition
                          && method.GetGenericArguments().Length == 2
                          && method.GetParameters().Length == 2);
            var genericParams = orderByMethod.GetGenericArguments();
            var orderParams = orderByMethod.GetParameters();
            var makeOrderBy = orderByMethod
                .MakeGenericMethod(typeof(TEntity), type);
            object result = makeOrderBy
                .Invoke(null, new object[] { source, lambda });
            return (IQueryable<TEntity>)result;
        }

        private static readonly MethodInfo StringComparisonExpressionMethodInfo =
            typeof(string).GetMethod("Compare", new Type[] {
                typeof(string), typeof(string), typeof(StringComparison)
            });

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

    }
}
