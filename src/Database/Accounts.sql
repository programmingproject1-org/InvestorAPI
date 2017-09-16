-- Table: public."Accounts"

-- DROP TABLE public."Accounts";

CREATE TABLE public."Accounts"
(
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Balance" money NOT NULL,
    CONSTRAINT "Accounts_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Accounts_Users" FOREIGN KEY ("UserId")
        REFERENCES public."Users" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Accounts"
    OWNER to wmjepiakfzqvwv;

-- Index: fki_Accounts_Users

-- DROP INDEX public."fki_Accounts_Users";

CREATE INDEX "fki_Accounts_Users"
    ON public."Accounts" USING btree
    (UserId)
    TABLESPACE pg_default;
