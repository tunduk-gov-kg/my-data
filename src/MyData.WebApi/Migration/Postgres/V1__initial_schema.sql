create table "Requests"
(
    "Id"                   bigserial                   not null primary key,
    "CreatedAt"            timestamp without time zone not null,
    "ServiceInvokedAt"     timestamp without time zone not null,

    "ClientXRoadInstance"  varchar(20)                 not null,
    "ClientMemberClass"    varchar(5)                  not null,
    "ClientMemberCode"     varchar(10)                 not null,
    "ClientSubsystemCode"  varchar(200),

    "ServiceXRoadInstance" varchar(20)                 not null,
    "ServiceMemberClass"   varchar(5)                  not null,
    "ServiceMemberCode"    varchar(10)                 not null,
    "ServiceSubsystemCode" varchar(200),
    "ServiceCode"          varchar(500)                not null,
    "ServiceVersion"       varchar(10),

    "XRequestId"           varchar(100),
    "MessageId"            varchar(100)                not null,
    "UserId"               varchar(100)                not null,
    "MessageIssue"         text,
    "Pin"                  varchar(15)                 not null
);

create index service_invoked_at_ix on "Requests" ("ServiceInvokedAt");