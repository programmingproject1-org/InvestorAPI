#!/usr/bin/env python
# -*- coding: utf-8 -*-

from .user_investor import User
from .endpoints.registration import Registration
from .endpoints.authentication import Authentication
from .endpoints.deletion import Deletion
from .endpoints.view_details import ViewDetails



API_URL = "https://investor-api.herokuapp.com/api/1.0"
REGISTER_URL = API_URL + "/users"
AUTHENTICATION_URL = API_URL + "/token"
DELETION_URL = API_URL + "/users"
VIEW_DETAILS_URL = API_URL + "/users"


class ApiFacade:
	def __init__(self, user, token = None):
		self.user = user
		self.token = token

	def register_user(self):
		registration = Registration(REGISTER_URL, self.user)
		return registration.get_outcome()

	def authenticate_user(self):
		authentication = Authentication(AUTHENTICATION_URL, self.user)
		if authentication.get_outcome().is_success:
			self.token = authentication.get_token()
		return authentication.get_outcome()

	def delete_user(self):
		deletion = Deletion(DELETION_URL, self.user, self.token)
		return deletion.get_outcome()

	def view_details(self):
		view_details = ViewDetails(VIEW_DETAILS_URL, self.user, self.token)
		return view_details.get_outcome()

