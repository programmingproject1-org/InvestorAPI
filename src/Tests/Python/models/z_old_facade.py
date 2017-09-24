API_URL = "https://investor-api.herokuapp.com/api/1.0"

class API_Facade():
	def make_header(content_type = "application/json", token = None):
		if token is not None:
			return {"Content-Type": content_type, "Authorization": "Bearer " + token}
		else:
			return {"Content-Type": content_type}

	def register(displayName, email, password):
		endpoint = "/users"
		url = API_URL + endpoint

		payload = {"displayName": displayName, "email": email, "password": password}
		response = requests.post(url, data = json.dumps(payload), headers = API.make_header())

		if response.status_code == 201:
			return {"success": True, "status": response.status_code, "body": None}
		else:
			response_body = response.json()
			return {"success": False, "status": response.status_code, "body": response_body}

	def sign_in(email, password):
		endpoint = "/token"
		url = API_URL + endpoint

		payload = {"email": email, "password": password}
		response = requests.post(url, data = json.dumps(payload), headers = API.make_header())

		if response.status_code == 200:
			response_body = response.json()
			return {"success": True, "status": response.status_code, "body": response_body}
		else:
			return {"success": False, "status": response.status_code, "body": None}

	def delete_user(token):
		endpoint = "/users"
		url = API_URL + endpoint

		response = requests.delete(url, headers = API.make_header(token = token))

		if response.status_code == 204:
			return {"success": True, "status": response.status_code}
		else:
			return {"success": False, "status": response.status_code}

	def clean_up(email, password):
		result = API.sign_in(email, password)
		token = result["body"]["accessToken"]
		API.delete_user(token)

	def get_user_details(token):
		endpoint = "/users"
		url = API_URL + endpoint

		response = requests.get(url, headers = API.make_header(token = token))

		if response.status_code == 200:
			response_body = response.json()
			return {"success": True, "status": response.status_code, "body": response_body}
		else:
			return {"success": True, "status": response.status_code, "body": None}

	def buy_shares(token, account_id, symbol, quantity, nonce, side = "BUY"):
		endpoint = "/accounts/{0}/orders".format(account_id)
		url = API_URL + endpoint

		payload = {"side": side, "symbol": symbol, "quantity": quantity, "nonce": nonce}
		response = requests.post(url, data = json.dumps(payload), headers = API.make_header(token = token))

		if response.status_code == 201:
			response_body = response.json()
			return {"success": True, "status": response.status_code, "body": response_body}
		else:
			return {"success": False, "status": response.status_code, "body": None}


class ApiFacade():
	def __init__(user, token = None):
		self.user = user
		self.token = token

	def register():
		