#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json

from api_client.response_wrappers.registration_response_wrapper import RegistrationResponseWrapper

class RegistrationRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/users"

	def __init__(self, session, displayName, email, password):
		self.make_request(displayName, email, password)
		self.session = session
		self.displayName = displayName
		self.email = email
		self.password = password

	def make_request(self, displayName, email, password):
		user_info = {"displayName": displayName, "email": email, "password": password}
		headers = {"Content-Type": "application/json", "charset": "UTF-8"}
		payload = json.dumps(user_info, ensure_ascii = False).encode('utf8')
		super().__init__('POST', url = self.URL, headers = headers, data = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = RegistrationResponseWrapper(response)
		response.close()
		return wrapped_response