﻿using YAF.Lucene.Net.Analysis;

namespace YAF.Lucene.Net.Search.Highlight
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

    /// <summary> <see cref="IFragmenter"/> implementation which does not fragment the text.
    /// This is useful for highlighting the entire content of a document or field.
    /// </summary>
    public class NullFragmenter : IFragmenter
    {
        public virtual void Start(string originalText, TokenStream tokenStream)
        { }

        public virtual bool IsNewFragment()
        {
            return false;
        }
    }
}