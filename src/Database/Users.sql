CREATE TABLE public."Users"
(
    "Id" uuid NOT NULL,
    "DisplayName" text COLLATE pg_catalog."default" NOT NULL,
    "Email" text COLLATE pg_catalog."default" NOT NULL,
    "HashedPassword" text COLLATE pg_catalog."default" NOT NULL,
    "Level" smallint NOT NULL,
    CONSTRAINT "Users_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Users_Email_Unique" UNIQUE ("Email")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Users"
    OWNER to wmjepiakfzqvwv;
