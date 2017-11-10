-- Table: public."Watchlists"

-- DROP TABLE public."Watchlists";

CREATE TABLE public."Watchlists"
(
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Symbols" character varying[] COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Watchlists_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Watchlists_Users" FOREIGN KEY ("UserId")
        REFERENCES public."Users" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Watchlists"
    OWNER to wmjepiakfzqvwv;

-- Index: fki_Watchlists_Users

-- DROP INDEX public."fki_Watchlists_Users";

CREATE INDEX "fki_Watchlists_Users"
    ON public."Watchlists" USING btree
    (UserId)
    TABLESPACE pg_default;
