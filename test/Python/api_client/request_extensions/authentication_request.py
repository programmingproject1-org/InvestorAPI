#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json

from api_client.response_wrappers.authentication_response_wrapper import AuthenticationResponseWrapper

class AuthenticationRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/token"

	def __init__(self, session, email, password):
		self.make_request(email, password)
		self.session = session
		self.email = email
		self.password = password

	def make_request(self, email, password):
		user_info = {"email": email, "password": password}
		headers = {"Content-Type": "application/json", "charset": "UTF-8"}
		payload = json.dumps(user_info, ensure_ascii = False).encode('utf8')
		super().__init__('POST', url = self.URL, headers = headers, data = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = AuthenticationResponseWrapper(response)
		response.close()
		return wrapped_response