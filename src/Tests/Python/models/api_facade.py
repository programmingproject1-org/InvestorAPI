#!/usr/bin/env python
# -*- coding: utf-8 -*-

from .endpoints.registration import Registration
from .endpoints.authentication import Authentication
from .endpoints.deletion import Deletion
from .endpoints.view_details import ViewDetails
from .endpoints.current_quotes import CurrentQuotes

API_URL = "https://investor-api.herokuapp.com/api/1.0"
REGISTER_URL = API_URL + "/users"
AUTHENTICATION_URL = API_URL + "/token"
DELETION_URL = API_URL + "/users"
VIEW_DETAILS_URL = API_URL + "/users"
CURRENT_QUOTES_URL = API_URL + "/shares/quotes"

class ApiFacade:
	def __init__(self):
		pass

	def register_user(self, displayName, email, password):
		registration = Registration(REGISTER_URL, displayName, email, password)
		return registration.get_outcome()

	def authenticate_user(self, email, password):
		authentication = Authentication(AUTHENTICATION_URL, email, password)
		return authentication.get_outcome()

	def delete_user(self, token):
		deletion = Deletion(DELETION_URL, token)
		return deletion.get_outcome()

	def view_details(self, token):
		view_details = ViewDetails(VIEW_DETAILS_URL, token)
		return view_details.get_outcome()

	def get_current_quotes(self, symbols, token):
		current_quotes = CurrentQuotes(CURRENT_QUOTES_URL, symbols, token)
		return current_quotes.get_outcome()

