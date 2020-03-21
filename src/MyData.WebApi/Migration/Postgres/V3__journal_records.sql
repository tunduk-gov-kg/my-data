create table "Journal"
(
    "Id"               bigserial                   not null primary key,
    "CreatedAt"        timestamp without time zone not null,

    "DbHost"           varchar(200)                not null,
    "DbPort"           int                         not null,
    "FromIdInclusive"  bigint                      not null,
    "Limit"            int                         not null,
    "LastRecordId"     bigint                      not null,
    "ParsedCount"      int                         not null,
    "Succeeded"        bool                        not null,
    "ErrorCode"        varchar(200),
    "ErrorDescription" text
);
