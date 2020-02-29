create table "Services"
(
    "Id"             bigserial                   not null primary key,
    "CreatedAt"      timestamp without time zone not null,

    "XRoadInstance"  varchar(20)                 not null,
    "MemberClass"    varchar(5)                  not null,
    "MemberCode"     varchar(10)                 not null,
    "SubsystemCode"  varchar(200),
    "ServiceCode"    varchar(500)                not null,
    "ServiceVersion" varchar(10)
);

create unique index service_domain_id_ix on "Services" ("XRoadInstance", "MemberClass", "MemberCode", "SubsystemCode",
                                                        "ServiceCode", "ServiceVersion");