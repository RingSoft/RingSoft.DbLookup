using RingSoft.DbLookup.Lookup;
using System;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupSearchForHostFactory
    {
        internal LookupSearchForHost CreateSearchForHost(LookupColumnDefinitionBase columnDefinition)
        {
            var hostId = columnDefinition.SearchForHostId;
            if (hostId == null)
                hostId = columnDefinition.DataType.ConvertFieldDataTypeToSearchForHostId();

            var searchForHost = CreateSearchForHost(hostId);
            searchForHost.InternalInitialize(columnDefinition);

            return searchForHost;
        }

        protected virtual LookupSearchForHost CreateSearchForHost(int? hostId)
        {
            if (hostId == LookupColumnDefinitionBase.SearchForStringHostId)
                return new LookupSearchForStringHost();
            else if (hostId == LookupColumnDefinitionBase.SearchForDateHostId)
                return new LookupSearchForDateHost();

            //throw new Exception($"SearchForHost not created for HostId {hostId}");
            return new LookupSearchForStringHost();
        }
    }
}
