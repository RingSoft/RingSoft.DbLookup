// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 02-16-2023
//
// Last Modified By : petem
// Last Modified On : 02-16-2023
// ***********************************************************************
// <copyright file="SqlData.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Class SqlData.
    /// </summary>
    public class SqlData
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; private set; }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <value>The field value.</value>
        public string FieldValue { get; private set;}

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ValueTypes ValueType { get; private set; }

        /// <summary>
        /// Gets the type of the date.
        /// </summary>
        /// <value>The type of the date.</value>
        public DbDateTypes DateType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlData"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="dateType">Type of the date.</param>
        public SqlData(string fieldName, string fieldValue, ValueTypes valueType, DbDateTypes dateType = DbDateTypes.DateOnly)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
            ValueType = valueType;
            DateType = dateType;
        }
    }

    /// <summary>
    /// Class UpdateDataStatement.
    /// </summary>
    public class UpdateDataStatement
    {
        /// <summary>
        /// Gets the SQL datas.
        /// </summary>
        /// <value>The SQL datas.</value>
        public IReadOnlyList<SqlData> SqlDatas  => _sqlDatas.AsReadOnly();

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The primary key value.</value>
        public PrimaryKeyValue PrimaryKeyValue { get; private set; }

        /// <summary>
        /// The SQL datas
        /// </summary>
        private List<SqlData> _sqlDatas = new List<SqlData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDataStatement"/> class.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        public UpdateDataStatement(PrimaryKeyValue primaryKeyValue)
        {
            PrimaryKeyValue = primaryKeyValue;
        }

        /// <summary>
        /// Adds the SQL data.
        /// </summary>
        /// <param name="sqlData">The SQL data.</param>
        public void AddSqlData(SqlData sqlData)
        {
            _sqlDatas.Add(sqlData);
        }
    }

    /// <summary>
    /// Class InsertDataStatement.
    /// </summary>
    public class InsertDataStatement
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; private set; }

        /// <summary>
        /// Gets the SQL datas.
        /// </summary>
        /// <value>The SQL datas.</value>
        public IReadOnlyList<SqlData> SqlDatas => _sqlDatas.AsReadOnly();

        /// <summary>
        /// The SQL datas
        /// </summary>
        private List<SqlData> _sqlDatas = new List<SqlData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertDataStatement"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public InsertDataStatement(TableDefinitionBase tableDefinition)
        {
            TableDefinition = tableDefinition;
        }
        /// <summary>
        /// Adds the SQL data.
        /// </summary>
        /// <param name="sqlData">The SQL data.</param>
        public void AddSqlData(SqlData sqlData)
        {
            _sqlDatas.Add(sqlData);
        }

    }
}
