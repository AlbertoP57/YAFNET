﻿using YAF.Lucene.Net.Documents;
using YAF.Lucene.Net.QueryParsers.Flexible.Core.Config;
using System.Collections.Generic;

namespace YAF.Lucene.Net.QueryParsers.Flexible.Standard.Config
{
    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    /// <summary>
    /// This listener listens for every field configuration request and assign a
    /// <see cref="ConfigurationKeys.DATE_RESOLUTION"/> to the equivalent <see cref="FieldConfig"/> based
    /// on a defined map: fieldName -> <see cref="DateTools.Resolution"/> stored in
    /// <see cref="ConfigurationKeys.FIELD_DATE_RESOLUTION_MAP"/>.
    /// </summary>
    /// <seealso cref="ConfigurationKeys.DATE_RESOLUTION"/>
    /// <seealso cref="ConfigurationKeys.FIELD_DATE_RESOLUTION_MAP"/>
    /// <seealso cref="FieldConfig"/>
    /// <seealso cref="IFieldConfigListener"/>
    public class FieldDateResolutionFCListener : IFieldConfigListener
    {
        private QueryConfigHandler config = null;

        public FieldDateResolutionFCListener(QueryConfigHandler config)
        {
            this.config = config;
        }

        public virtual void BuildFieldConfig(FieldConfig fieldConfig)
        {
            DateTools.Resolution? dateRes = null;
            IDictionary<string, DateTools.Resolution?> dateResMap = this.config.Get(ConfigurationKeys.FIELD_DATE_RESOLUTION_MAP);

            if (dateResMap != null)
            {
                dateResMap.TryGetValue(fieldConfig.Field, out dateRes);
            }

            if (dateRes == null)
            {
                dateRes = this.config.Get(ConfigurationKeys.DATE_RESOLUTION);
            }

            if (dateRes != null)
            {
                fieldConfig.Set(ConfigurationKeys.DATE_RESOLUTION, dateRes.Value);
            }
        }
    }
}
