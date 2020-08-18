﻿using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class PersonConfiguration : ResourceTypeBuilder<Person>
    {
        public PersonConfiguration()
        {
            // Relationships
            this.ToManyRelationship<Article>("articles");
            this.ToManyRelationship<Comment>("comments");

            // Ignore
            this.Attribute(x => x.Articles).Ignore();
            this.Attribute(x => x.Comments).Ignore();
        }
    }
}