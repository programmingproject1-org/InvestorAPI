-- Table: public."Positions"

-- DROP TABLE public."Positions";

CREATE TABLE public."Positions"
(
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "Symbol" character varying(10) COLLATE pg_catalog."default" NOT NULL,
    "Quantity" integer NOT NULL,
    "AveragePrice" money NOT NULL,
    CONSTRAINT "Positions_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Positions_Accounts" FOREIGN KEY ("AccountId")
        REFERENCES public."Accounts" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Positions"
    OWNER to wmjepiakfzqvwv;

-- Index: fki_Positions_Accounts

-- DROP INDEX public."fki_Positions_Accounts";

CREATE INDEX "fki_Positions_Accounts"
    ON public."Positions" USING btree
    (AccountId)
    TABLESPACE pg_default;
